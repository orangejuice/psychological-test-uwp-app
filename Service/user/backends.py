

class TokenAuthenticationBackend:
    keyword = 'Token'
    model = None

    def get_model(self):
        if self.model is not None:
            return self.model
        from rest_framework.authtoken.models import Token
        return Token

    def authenticate(self, request, **credentials):
        model = self.get_model()
        try:
            token = model.objects.select_related('user').get(key=credentials['token'])
        except model.DoesNotExist:
            return None

        if not token.user.is_active:
            return None

        return token.user
