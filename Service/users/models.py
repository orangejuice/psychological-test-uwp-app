from django.contrib.auth.models import User
from django.db import models

DEFAULT_AVATAR = 'static/images/avatar/553f736466666473667364666364571b.jpg'


# Create your models here.
class UserProfile(User):
    avatar = models.ImageField(null=True, blank=True, default=DEFAULT_AVATAR, upload_to='static/images/avatar')
