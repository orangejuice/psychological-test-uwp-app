from django.contrib.auth.models import Group
from rest_framework import viewsets
from rest_framework.decorators import action
from rest_framework.permissions import IsAuthenticated

from user.models import UserProfile
from user.serializers import UserSerializer, GroupSerializer


class UserViewSet(viewsets.ReadOnlyModelViewSet):
    queryset = UserProfile.objects.all().order_by('-date_joined')
    serializer_class = UserSerializer

    @action(methods=['GET'], permission_classes=[IsAuthenticated], detail=False)
    def me(self, request, *args, **kwargs):
        self.kwargs.update(pk=request.user.id)
        return self.retrieve(request, *args, **kwargs)


class GroupViewSet(viewsets.ReadOnlyModelViewSet):
    queryset = Group.objects.all()
    serializer_class = GroupSerializer
