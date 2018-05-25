from django.contrib.auth import authenticate
from django.contrib.auth.models import AnonymousUser
from django.http import HttpResponseRedirect
from django.utils.deprecation import MiddlewareMixin
from rest_framework.authentication import get_authorization_header
from django.contrib.auth import login, get_backends


class TokenAuthenticationMiddleware(MiddlewareMixin):
    keyword = 'Token'

    def process_request(self, request):
        try:
            auth = request.COOKIES['Authorization'].split()

            if not auth or auth[0].lower() != self.keyword.lower():
                return None
            if len(auth) == 1 or len(auth) > 2:
                return None
            token = auth[1]
        except:
            return None
        #     auth = get_authorization_header(request).split()
        #
        #     if not auth or auth[0].lower() != self.keyword.lower().encode():
        #         return None
        #     if len(auth) == 1 or len(auth) > 2:
        #         return None
        #     try:
        #         token = auth[1].decode()
        #     except UnicodeError:
        #         return None
        user = authenticate(token=token)
        # backend = get_backends()[0]
        # user.backend = "%s.%s" % (backend.__module__, backend.__class__.__name__)
        # login(request, user)
        # print("user login")
        request.user = user
