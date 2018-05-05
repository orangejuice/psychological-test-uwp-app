from rest_framework import serializers

from eval.models import Scale, ScaleItem, ScaleOption, ScaleConclusion, ScaleResult, ScaleRecord, ScaleItemOpt


class ScaleListSerializer(serializers.HyperlinkedModelSerializer):
    class Meta:
        model = Scale
        fields = ('url', 'title', 'introduction', 'thumbnail', 'created', 'is_top')


class ScaleOptionSerializer(serializers.ModelSerializer):
    class Meta:
        model = ScaleOption
        fields = ('key', 'value')


class ScaleResultSerializer(serializers.ModelSerializer):
    class Meta:
        model = ScaleResult
        fields = ('key', 'value')


class ScaleItemOptSerializer(serializers.ModelSerializer):
    key = serializers.ReadOnlyField(source='option.key')
    value = serializers.ReadOnlyField(source='option.value')
    score = serializers.ReadOnlyField(source='option.score')
    bonus = ScaleResultSerializer(read_only=True)

    class Meta:
        model = ScaleItemOpt
        fields = ('key', 'value', 'bonus', 'score')


class ScaleItemSerializer(serializers.HyperlinkedModelSerializer):
    opts = ScaleOptionSerializer(many=True, read_only=True)

    class Meta:
        model = ScaleItem
        fields = ('url', 'sn', 'question', 'opts')


class ScaleItemCalSerializer(serializers.ModelSerializer):
    opts = ScaleItemOptSerializer(many=True, source='scaleitemopt_set', read_only=True)

    class Meta:
        model = ScaleItem
        fields = ('sn', 'question', 'opts')


class ScaleSerializer(serializers.HyperlinkedModelSerializer):
    items = serializers.SerializerMethodField()

    class Meta:
        model = Scale
        fields = ('url', 'title', 'items')

    def get_items(self, obj):
        qs = ScaleItem.objects.filter(scale=obj.pk)
        items = ScaleItemSerializer(qs, many=True, context=self.context).data
        return items


class ScaleConclusionSerializer(serializers.HyperlinkedModelSerializer):
    class Meta:
        model = ScaleConclusion
        fields = ('url', 'key', 'description')


class ScaleRecordSerializer(serializers.HyperlinkedModelSerializer):
    class Meta:
        model = ScaleRecord
        fields = ('url', 'score')


class ScaleRecordAddSerializer(serializers.ModelSerializer):
    class Meta:
        model = ScaleRecord
        fields = ('user', 'scale', 'chose', 'score', 'result')
