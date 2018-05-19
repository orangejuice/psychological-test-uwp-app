from django.contrib import admin
from rest_framework.authtoken.admin import TokenAdmin

from user.models import UserProfile

TokenAdmin.raw_id_fields = ('user',)
admin.site.register(UserProfile)
