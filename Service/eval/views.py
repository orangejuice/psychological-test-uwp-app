from rest_framework import viewsets, status, mixins
from rest_framework.response import Response
from rest_framework.settings import api_settings

from eval.models import Scale, ScaleItem, ScaleOption, ScaleRecord, ScaleConclusion, ScaleResult
from eval.serializers import ScaleListSerializer, ScaleSerializer, ScaleItemSerializer, ScaleOptionSerializer, \
    ScaleConclusionSerializer, ScaleResultSerializer, ScaleRecordAddSerializer, ScaleRecordSerializer, \
    ScaleItemCalSerializer


# Create your views here.
class ScaleViewSet(viewsets.GenericViewSet):
    queryset = Scale.objects.all()

    def list(self, request):
        queryset = Scale.objects.all()

        page = self.paginate_queryset(queryset)
        if page is not None:
            serializer = ScaleListSerializer(page, many=True, context={'request': request})
            return self.get_paginated_response(serializer.data)

        serializer = ScaleListSerializer(queryset, many=True, context={'request': request})
        return Response(serializer.data)

    @staticmethod
    def retrieve(request, pk=None):
        queryset = Scale.objects.get(pk=pk)
        serializer = ScaleSerializer(queryset, context={'request': request})
        return Response(serializer.data)


class ScaleItemViewSet(viewsets.ReadOnlyModelViewSet):
    queryset = ScaleItem.objects.all()
    serializer_class = ScaleItemSerializer


class ScaleOptionViewSet(viewsets.ReadOnlyModelViewSet):
    queryset = ScaleOption.objects.all()
    serializer_class = ScaleOptionSerializer


class ScaleResultViewSet(viewsets.GenericViewSet):
    queryset = ScaleResult.objects.all()
    serializer_class = ScaleResultSerializer

    @staticmethod
    def retrieve(request, pk=None):  # , scale=None, key=None
        queryset = ScaleConclusion.objects.get(pk=pk)
        #  if pk is not None \
        #             else ScaleConclusion.objects.get(scale=scale, key=key)
        serializer = ScaleConclusionSerializer(queryset, context={'request': request})
        return Response(serializer.data)


class ScaleConclusionViewSet(viewsets.ReadOnlyModelViewSet):
    queryset = ScaleConclusion.objects.all()
    serializer_class = ScaleConclusionSerializer


class ScaleRecordViewSet(mixins.ListModelMixin, mixins.RetrieveModelMixin, viewsets.GenericViewSet):
    queryset = ScaleRecord.objects.all()
    serializer_class = ScaleRecordSerializer

    def get_queryset(self):
        user = self.request.user.pk
        return ScaleRecord.objects.filter(user=user)

    def create(self, request):
        request.data['score'] = self.get_score(request.data['scale'], request.data['opts'])
        request.data['result'] = self.get_result(request.data['scale'], request.data['score'])
        request.data['chose'] = request.data['opts']
        request.data['user'] = request.user.pk
        serializer = ScaleRecordAddSerializer(data=request.data, context={'request', request})
        serializer.is_valid()
        self.perform_create(serializer)
        headers = self.get_success_headers(serializer.data)
        return Response(serializer.data, status=status.HTTP_201_CREATED, headers=headers)

    def get_score(self, scale, opts):
        item_score = {}
        score = {}
        # TODO 答题校验
        for sn, chose in opts.items():
            question = ScaleItem.objects.filter(scale=scale, sn=sn)[0]
            ser = ScaleItemCalSerializer(question, context={'request', self.request})
            for opt in ser.data['opts']:
                if opt['key'] == chose and opt['bonus'] is not None:
                    result = opt['bonus']['key']
                    point = opt['score']
                    item_score[sn] = point
                    if result in score.keys():
                        score[result] += point
                    else:
                        score[result] = point
                    break
        # Anchor    在40题中挑出三个得分最高的项目 + 4
        if scale == 3:
            sns = sorted(item_score, key=item_score.get, reverse=True)[:3]
            for sn in sns:
                question = ScaleItem.objects.filter(scale=scale, sn=sn)[0]
                ser = ScaleItemCalSerializer(question, context={'request', self.request})
                result = ser.data['opts'][0]['bonus']['key']
                score[result] += 4
        return score

    @staticmethod
    def get_result(scale, score):
        # {'J': 2, 'P':4}
        # MBTI      E I / S N / T F / J P 选4
        # HOLLAND   R I A S E C 选3
        # Anchor    TF  GM  AU  SE  EC  SV  CH  LS 选1
        fin_code = ''
        if scale == 1:
            fin_code += 'E' if score['E'] >= score['I'] else 'I'
            fin_code += 'S' if score['S'] >= score['N'] else 'N'
            fin_code += 'T' if score['T'] >= score['F'] else 'F'
            fin_code += 'J' if score['J'] >= score['P'] else 'P'
        elif scale == 2:
            fin_code = fin_code.join(sorted(score, key=score.get, reverse=True)[:3])
        elif scale == 3:
            fin_code = fin_code.join(sorted(score, key=score.get, reverse=True)[:1])
        return fin_code

    @staticmethod
    def perform_create(serializer):
        serializer.save()

    @staticmethod
    def get_success_headers(data):
        try:
            return {'Location': str(data[api_settings.URL_FIELD_NAME])}
        except (TypeError, KeyError):
            return {}
