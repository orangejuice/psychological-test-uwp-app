from rest_framework import serializers

from eval.models import Scale, ScaleItem, ScaleOption, ScaleConclusion, ScaleResult, ScaleRecord


class ScaleListSerializer(serializers.HyperlinkedModelSerializer):
    class Meta:
        model = Scale
        fields = ('url', 'title', 'introduction', 'thumbnail', 'created', 'is_top')


class ScaleOptionSerializer(serializers.HyperlinkedModelSerializer):
    class Meta:
        model = ScaleOption
        fields = ('url', 'key', 'value')


class ScaleItemSerializer(serializers.HyperlinkedModelSerializer):
    options = ScaleOptionSerializer(many=True, source='opts', read_only=True)

    class Meta:
        model = ScaleItem
        fields = ('url', 'sn', 'question', 'options')


class ScaleSerializer(serializers.HyperlinkedModelSerializer):
    items = serializers.SerializerMethodField()

    class Meta:
        model = Scale
        fields = ('url', 'title', 'items')

    def get_items(self, obj):
        qs = ScaleItem.objects.filter(scale=obj.pk)
        items = ScaleItemSerializer(qs, many=True, context=self.context).data
        return items


class ScaleResultSerializer(serializers.HyperlinkedModelSerializer):
    class Meta:
        model = ScaleResult
        fields = ('url', 'key', 'value')


class ScaleConclusionSerializer(serializers.HyperlinkedModelSerializer):
    class Meta:
        model = ScaleConclusion
        fields = ('url', 'key', 'description')


class ScaleRecordSerializer(serializers.HyperlinkedModelSerializer):
    conclusion = ScaleConclusionSerializer(read_only=True)

    class Meta:
        model = ScaleRecord
        fields = ('url', 'final', 'conclusion')


class ScaleRecordAddSerializer(serializers.HyperlinkedModelSerializer):
    class Meta:
        model = ScaleRecord
        fields = ('user', 'scale', 'chose',)
