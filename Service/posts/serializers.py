from rest_framework import serializers

from posts.models import Article, Category


class PostSerializer(serializers.HyperlinkedModelSerializer):
    author_name = serializers.ReadOnlyField(source='author.username')
    cate_name = serializers.ReadOnlyField(source='cate.name')

    class Meta:
        model = Article
        fields = ('url', 'title', 'cate', 'cate_name', 'thumbnail', 'author',
                  'author_name', 'content', 'created', 'updated',)


class CategorySerializer(serializers.HyperlinkedModelSerializer):
    class Meta:
        model = Category
        fields = ('url', 'name', 'updated')
