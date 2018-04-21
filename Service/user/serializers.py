from django.contrib.auth.models import Group
from rest_framework import serializers

from user.models import UserProfile


class UserSerializer(serializers.HyperlinkedModelSerializer):
    groups = serializers.SlugRelatedField(many=True, read_only=True, slug_field="name")

    class Meta:
        model = UserProfile
        fields = ('url', 'username', 'email', 'groups', 'avatar')


class GroupSerializer(serializers.HyperlinkedModelSerializer):
    class Meta:
        model = Group
        fields = ('url', 'name')
