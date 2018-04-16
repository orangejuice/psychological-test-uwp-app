from django.db import models


class Scale(models.Model):
    title = models.CharField(max_length=100, blank=False)
    introduction = models.TextField()
    thumbnail = models.ImageField(null=True, blank=True, default=None, upload_to='static/images/thumb')
    is_top = models.BooleanField('top status', default=False)
    created = models.DateTimeField(auto_now=True)

    class Meta:
        ordering = ['-created']

    def __str__(self):
        return str(self.title)


class ScaleOption(models.Model):
    scale = models.ForeignKey(Scale, on_delete=models.CASCADE)
    name = models.CharField(max_length=100, blank=False)
    score = models.IntegerField()
