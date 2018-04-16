from django.contrib.auth.models import Group
from rest_framework import serializers

from users.models import UserProfile


class UserSerializer(serializers.HyperlinkedModelSerializer):
    group = serializers.ReadOnlyField(source='groups.name')

    class Meta:
        model = UserProfile
        fields = ('url', 'username', 'email', 'group', 'avatar')


class GroupSerializer(serializers.HyperlinkedModelSerializer):
    class Meta:
        model = Group
        fields = ('url', 'name')
