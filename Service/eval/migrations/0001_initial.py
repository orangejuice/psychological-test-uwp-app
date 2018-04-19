# Generated by Django 2.0.4 on 2018-04-18 08:56

import django.db.models.deletion
from django.db import migrations, models


class Migration(migrations.Migration):

    initial = True

    dependencies = [
    ]

    operations = [
        migrations.CreateModel(
            name='Scale',
            fields=[
                ('id', models.AutoField(auto_created=True, primary_key=True, serialize=False, verbose_name='ID')),
                ('title', models.CharField(max_length=100)),
                ('introduction', models.TextField()),
                ('thumbnail', models.ImageField(blank=True, default=None, null=True, upload_to='static/images/thumb')),
                ('is_top', models.BooleanField(default=False, verbose_name='top status')),
                ('created', models.DateTimeField(auto_now=True)),
            ],
            options={
                'ordering': ['-created'],
            },
        ),
        migrations.CreateModel(
            name='ScaleOption',
            fields=[
                ('id', models.AutoField(auto_created=True, primary_key=True, serialize=False, verbose_name='ID')),
                ('name', models.CharField(max_length=100)),
                ('score', models.IntegerField()),
                ('scale', models.ForeignKey(on_delete=django.db.models.deletion.CASCADE, to='eval.Scale')),
            ],
            options={
                'db_table': 'eval_scale_option',
            },
        ),
    ]
