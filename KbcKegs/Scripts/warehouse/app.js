(function () {
    'use strict';

    angular.module('app', ['angular-loading-bar']);
    angular.module('app').controller('WarehouseController', WarehouseController);

    WarehouseController.$inject = ['$scope', '$http', 'assets', 'addSuccessCallback'];
    function WarehouseController($scope, $http, assets, addSuccessCallback) {
        $scope.assets = assets;

        $scope.newDelivery = {
            orderId: -1,
            assets: []
        };

        $scope.selectedAsset = null;
        $scope.removeAsset = removeAsset;
        $scope.addSelectedAsset = addSelectedAsset;
        $scope.addDelivery = addDelivery;

        $scope.$watch('selectedAssetId', handleSelectedAssetIdChanged);

        function moveAsset(asset, fromArray, toArray) {
            for (var idx = 0; idx < fromArray.length; ++idx) {
                if (asset == fromArray[idx]) {
                    fromArray.splice(idx, 1);
                    break;
                }
            }

            toArray.push(asset);
            toArray.sort(function (a, b) { return a.id - b.id });
        };

        function removeAsset(asset) {
            if (null == asset) return;
            moveAsset(asset, $scope.newDelivery.assets, $scope.assets);
        };

        function addSelectedAsset() {
            var asset = $scope.selectedAsset;

            if (null == asset) return;
            moveAsset(asset, $scope.assets, $scope.newDelivery.assets);
        };

        function handleSelectedAssetIdChanged(newValue, oldValue) {
            var newAsset = null;
            for (var idx = 0; idx < $scope.assets.length; ++idx) {
                var asset = $scope.assets[idx];
                if (asset.id == $scope.selectedAssetId) {
                    newAsset = asset;
                    break;
                }
            }

            $scope.selectedAsset = newAsset;
        };

        function addDelivery() {
            $http.post('/api/deliveries/', $scope.newDelivery)
                 .then(handleAddSuccess, handleAddFailure);

            function handleAddSuccess(response) {
                console.log('added successfully');
                addSuccessCallback();
            };

            function handleAddFailure(response) {
                console.log('error getting API data', response);
                if (!angular.isObject(response.data) || !response.data.message) {
                    $q.reject({
                        statusMessage: response.statusText,
                        errorMessage: 'An unknown error occurred',
                        errorMessageDetail: ''
                    });
                } else {
                    $q.reject({
                        statusText: response.statusText,
                        message: response.data.message,
                        messageDetail: response.data.messageDetail
                    });
                }
            };
        };
    }
})();