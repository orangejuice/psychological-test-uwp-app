from django.contrib.auth.models import AbstractUser
from django.db import models

DEFAULT_AVATAR = 'avatar/default.jpg'


# Create your models here.
class UserProfile(AbstractUser):
    avatar = models.ImageField(null=True, blank=True,
                               default=DEFAULT_AVATAR, upload_to='avatar')

    class Meta:
        db_table = 'auth_user'
        verbose_name = '用户'
        verbose_name_plural = '用户'
