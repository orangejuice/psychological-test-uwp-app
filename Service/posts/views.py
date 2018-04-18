from django_comments.models import Comment
from rest_framework import viewsets

from posts.models import Article, Category
from posts.serializers import PostSerializer, CategorySerializer, CommentSerializer


class PostViewSet(viewsets.ModelViewSet):
    queryset = Article.objects.all()
    serializer_class = PostSerializer


class CategoryViewSet(viewsets.ModelViewSet):
    queryset = Category.objects.all()
    serializer_class = CategorySerializer


class CommentViewSet(viewsets.ModelViewSet):
    queryset = Comment.objects.all()
    serializer_class = CommentSerializer
