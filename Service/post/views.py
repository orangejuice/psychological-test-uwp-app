from rest_framework import viewsets
from rest_framework.response import Response

from post.models import Article, Category
from post.serializers import PostListSerializer, CategorySerializer, PostDetailSerializer


class PostViewSet(viewsets.GenericViewSet):
    queryset = Article.objects.all()

    @staticmethod
    def list(request):
        queryset = Article.objects.all()
        serializer = PostListSerializer(queryset, many=True, context={'request': request})
        return Response(serializer.data)

    @staticmethod
    def retrieve(request, pk=None):
        queryset = Article.objects.get(pk=pk)
        serializer = PostDetailSerializer(queryset, context={'request': request})
        return Response(serializer.data)


class CategoryViewSet(viewsets.ReadOnlyModelViewSet):
    queryset = Category.objects.all()
    serializer_class = CategorySerializer
