from rest_framework import viewsets

from posts.models import Article, Category
from posts.serializers import PostSerializer, CategorySerializer


# Create your views here.
class PostViewSet(viewsets.ModelViewSet):
    queryset = Article.objects.all()
    serializer_class = PostSerializer


# Create your views here.
class CategoryViewSet(viewsets.ModelViewSet):
    queryset = Category.objects.all()
    serializer_class = CategorySerializer
