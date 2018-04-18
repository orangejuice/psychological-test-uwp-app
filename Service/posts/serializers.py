from django_comments.models import Comment
from rest_framework import serializers

from posts.models import Article, Category
from posts.tool import GeoNotFoundException, get_ip_area


class CommentSerializer(serializers.HyperlinkedModelSerializer):
    user_name = serializers.ReadOnlyField(source='user.username')

    class Meta:
        model = Comment
        fields = ('url', 'comment', 'user_name', 'user', 'submit_date')


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

    @staticmethod
    def get_comments_count(obj):
        return obj.comments.all().count()


class CategorySerializer(serializers.HyperlinkedModelSerializer):
    class Meta:
        model = Category
        fields = ('url', 'name', 'updated')
