from rest_framework import viewsets
from rest_framework.response import Response

from posts.models import Article, Category
from posts.serializers import PostListSerializer, CategorySerializer, PostDetailSerializer


# class PostListAPIView(generics.ListAPIView):
#     """List all comments for a given ContentType and object ID."""
#     serializer_class = PostListSerializer
#
#     def get_queryset(self):
#         qs = Article.objects.all()
#         return qs
#
#
# class PostDetailAPIView(generics.RetrieveAPIView):
#     queryset = Article.objects.all()
#     serializer_class = PostDetailSerializer
#     # lookup_field = 'slug'

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
