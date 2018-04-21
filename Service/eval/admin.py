# Register your models here.
from django.contrib import admin

from eval.models import Scale, ScaleOption, ScaleItem, ScaleResult, ScaleConclusion, ScaleRecord

admin.site.register(Scale)
admin.site.register(ScaleOption)
admin.site.register(ScaleItem)
admin.site.register(ScaleResult)
admin.site.register(ScaleConclusion)
admin.site.register(ScaleRecord)
