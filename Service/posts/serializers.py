from rest_framework import serializers

from posts.models import Article, Category, Comment
from posts.tool import GeoNotFoundException, get_ip_area


class CommentSerializer(serializers.ModelSerializer):
    replies_count = serializers.SerializerMethodField()
    author_name = serializers.ReadOnlyField(source='author.username')

    class Meta:
        model = Comment
        fields = ('url', 'content', 'author_name', 'author', 'article',
                  'replies_count', 'created')
        read_only_fields = ['reply_count', 'replies']

    def get_replies_count(self, obj):
        if obj.is_parent:
            return obj.children().count()
        return 0


class PostSerializer(serializers.HyperlinkedModelSerializer):
    author_name = serializers.ReadOnlyField(source='author.username')
    cate_name = serializers.ReadOnlyField(source='cate.name')
    comments = CommentSerializer(many=True, read_only=True)
    comments_count = serializers.SerializerMethodField()
    try:
        location = get_ip_area(serializers.ReadOnlyField(label='location'))
    except GeoNotFoundException:
        location = '未知位置'

    class Meta:
        model = Article
        fields = ('url', 'title', 'cate', 'cate_name', 'thumbnail', 'author', 'author_name',
                  'content', 'created', 'location', 'updated', 'comments', 'comments_count')

    def get_comments_count(self, obj):
        return obj.comments.all().count()


class CategorySerializer(serializers.HyperlinkedModelSerializer):
    class Meta:
        model = Category
        fields = ('url', 'name', 'updated')