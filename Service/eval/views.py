from rest_framework import viewsets, status
from rest_framework.response import Response
from rest_framework.settings import api_settings

from eval.models import Scale, ScaleItem, ScaleOption, ScaleRecord, ScaleConclusion, ScaleResult
from eval.serializers import ScaleListSerializer, ScaleSerializer, ScaleItemSerializer, ScaleOptionSerializer, \
    ScaleConclusionSerializer, ScaleResultSerializer, ScaleRecordAddSerializer


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
    queryset = ScaleRecord.objects.all()
    serializer_class = ScaleConclusionSerializer


class ScaleRecordViewSet(viewsets.GenericViewSet):
    def get_queryset(self):
        user = self.request.user
        return ScaleRecord.objects.filter(user=user)

    def create(self, request, *args, **kwargs):
        serializer = ScaleRecordAddSerializer(data=request.data)
        serializer.is_valid(raise_exception=True)
        serializer.save()
        headers = self.get_success_headers(serializer.data)
        return Response(serializer.data, status=status.HTTP_201_CREATED, headers=headers)

    def get_success_headers(self, data):
        try:
            return {'Location': str(data[api_settings.URL_FIELD_NAME])}
        except (TypeError, KeyError):
            return {}

    def retrieve(self, request, pk=None):
        pass
