# This script assumes you have a checkout of the monorepo (https://github.cds.internal.unity3d.com/unity/operate-services-sdk) at the same level as this repo

OPERATE_ROOT=../../../../../operate-services-sdk
SHARED_PACKAGE=Packages/Internal/com.unity.services.shared
STARBUCK2="dotnet run --project $OPERATE_ROOT/Tools/starbuck2/Starbuck2 --"

$STARBUCK2 $OPERATE_ROOT/$SHARED_PACKAGE/Editor/Assets              ../Editor/Authoring/Shared/Assets               --service RemoteConfig --namespace RemoteConfig.Authoring.Shared
$STARBUCK2 $OPERATE_ROOT/$SHARED_PACKAGE/Editor/Collections         ../Editor/Authoring/Shared/Collections          --service RemoteConfig --namespace RemoteConfig.Authoring.Shared
$STARBUCK2 $OPERATE_ROOT/$SHARED_PACKAGE/Editor/Crypto              ../Editor/Authoring/Shared/Crypto               --service RemoteConfig --namespace RemoteConfig.Authoring.Shared
$STARBUCK2 $OPERATE_ROOT/$SHARED_PACKAGE/Editor/DependencyInversion ../Editor/Authoring/Shared/DependencyInjection  --service RemoteConfig --namespace RemoteConfig.Authoring.Shared
$STARBUCK2 $OPERATE_ROOT/$SHARED_PACKAGE/Editor/Logging             ../Editor/Authoring/Shared/Logging              --service RemoteConfig --namespace RemoteConfig.Authoring.Shared
$STARBUCK2 $OPERATE_ROOT/$SHARED_PACKAGE/Editor/Threading           ../Editor/Authoring/Shared/Threading            --service RemoteConfig --namespace RemoteConfig.Authoring.Shared
