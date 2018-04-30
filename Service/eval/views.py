from rest_framework import viewsets
from rest_framework.response import Response

from eval.models import Scale, ScaleItem, ScaleOption
from eval.serializers import ScaleListSerializer, ScaleSerializer, ScaleItemSerializer, ScaleOptionSerializer


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


class ScaleItemViewSet(viewsets.ModelViewSet):
    queryset = ScaleItem.objects.all()
    serializer_class = ScaleItemSerializer


class ScaleOptionViewSet(viewsets.ModelViewSet):
    queryset = ScaleOption.objects.all()
    serializer_class = ScaleOptionSerializer
