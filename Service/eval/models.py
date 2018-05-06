from django.db import models

from jsonfield import JSONField
from user.models import UserProfile


class Scale(models.Model):
    title = models.CharField(max_length=100, blank=False)
    introduction = models.TextField(null=True, blank=True)
    thumbnail = models.ImageField(null=True, blank=True, default=None, upload_to='static/images/thumb')
    is_top = models.BooleanField('置顶', default=False)
    created = models.DateTimeField(auto_now_add=True)

    class Meta:
        db_table = 'eval_scale'
        ordering = ['-created']

    def __str__(self):
        return str(self.title)


class ScaleResult(models.Model):
    scale = models.ForeignKey(Scale, on_delete=models.CASCADE)
    key = models.CharField(max_length=100, blank=False)
    value = models.CharField(max_length=100, blank=False)

    class Meta:
        db_table = 'eval_result'


class ScaleOption(models.Model):
    # scale = models.ForeignKey(Scale, on_delete=models.CASCADE)
    key = models.IntegerField(blank=False, null=False, default=1)
    value = models.CharField(max_length=100, blank=False)
    score = models.IntegerField(default=1)

    class Meta:
        db_table = 'eval_option'

    def __str__(self):
        return str(self.value)


class ScaleItem(models.Model):
    scale = models.ForeignKey(Scale, on_delete=models.CASCADE)
    sn = models.IntegerField(default=1)
    question = models.TextField()
    opts = models.ManyToManyField(to=ScaleOption, related_name='options', through='ScaleItemOpt')

    class Meta:
        db_table = 'eval_item'
        ordering = ['sn']

    def __str__(self):
        return str(self.question[:20] + '...')


class ScaleItemOpt(models.Model):
    # 一个问题 -> 多个答案， 不同程度的对应不同的结果
    question = models.ForeignKey(to=ScaleItem, on_delete=models.CASCADE)
    option = models.ForeignKey(to=ScaleOption, on_delete=models.CASCADE)
    bonus = models.ForeignKey(ScaleResult, null=True, default=None, on_delete=models.SET_NULL)

    class Meta:
        db_table = 'eval_item_opts'


class ScaleConclusion(models.Model):
    scale = models.ForeignKey(Scale, on_delete=models.CASCADE)
    key = models.CharField(max_length=100, blank=False)
    description = models.TextField(blank=False)

    class Meta:
        db_table = 'eval_conclusion'


class ScaleRecord(models.Model):
    user = models.ForeignKey(UserProfile, on_delete=models.DO_NOTHING)
    scale = models.ForeignKey(Scale, on_delete=models.CASCADE)
    chose = JSONField('已选', null=False)
    score = JSONField('最终得分', null=False)
    result = models.CharField(max_length=100, blank=False)
    created = models.DateTimeField(auto_now_add=True)

    class Meta:
        db_table = 'eval_record'
        ordering = ['-created']
