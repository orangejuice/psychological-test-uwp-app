from rest_framework import viewsets, status
from rest_framework.response import Response
from rest_framework.settings import api_settings

from post.models import Article, Category, ArticleFavorite
from post.serializers import PostListSerializer, CategorySerializer, PostDetailSerializer, ArticleFavoriteSerializer, \
    ArticleFavoriteAddSerializer


class PostViewSet(viewsets.GenericViewSet):
    queryset = Article.objects.all()

    def list(self, request):
        queryset = Article.objects.all()

        page = self.paginate_queryset(queryset)
        if page is not None:
            serializer = PostListSerializer(page, many=True, context={'request': request})
            return self.get_paginated_response(serializer.data)

        serializer = PostListSerializer(queryset, many=True, context={'request': request})
        return Response(serializer.data)

    def retrieve(self, request, pk=None):
        queryset = Article.objects.get(pk=pk)
        serializer = PostDetailSerializer(queryset, context={'request': request})
        return Response(serializer.data)


class CategoryViewSet(viewsets.ReadOnlyModelViewSet):
    queryset = Category.objects.all()
    serializer_class = CategorySerializer
    pagination_class = None


class PostFavoriteViewSet(viewsets.GenericViewSet):
    queryset = ArticleFavorite.objects.all()
    serializer_class = ArticleFavoriteSerializer

    def list(self, request):
        user = request.user.pk
        queryset = ArticleFavorite.objects.filter(user=user)

        page = self.paginate_queryset(queryset)
        if page is not None:
            serializer = ArticleFavoriteSerializer(page, many=True, context={'request': request})
            return self.get_paginated_response(serializer.data)

        serializer = PostListSerializer(queryset, many=True, context={'request': request})
        return Response(serializer.data)

    def create(self, request):
        post = request.data['post']
        user = request.user.pk
        request.data['user'] = request.user.pk
        qs = ArticleFavorite.objects.filter(user=user, post=post)
        if qs is not None and qs.count() > 0:
            # there are favor record already. delete it.
            qs[0].delete()
            return Response(status=status.HTTP_204_NO_CONTENT)
        serializer = ArticleFavoriteAddSerializer(data=request.data, context={'request': request})
        serializer.is_valid(raise_exception=True)
        serializer.save()
        headers = self.get_success_headers(serializer.data)
        return Response(serializer.data, status=status.HTTP_201_CREATED, headers=headers)

    @staticmethod
    def get_success_headers(data):
        try:
            return {'Location': str(data[api_settings.URL_FIELD_NAME])}
        except (TypeError, KeyError):
            return {}
