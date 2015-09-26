(function () {
    'use strict';

    angular.module('app', ['angular-loading-bar']);
    angular.module('app').controller('NewDeliveryController', NewDeliveryController);
    angular.module('app').controller('NewCollectionController', NewCollectionController);

    NewDeliveryController.$inject = ['$scope', '$http', 'assetsAvailable', 'addSuccessCallback'];
    function NewDeliveryController($scope, $http, assetsAvailable, addSuccessCallback) {
        $scope.assetsAvailable = assetsAvailable;

        $scope.newDelivery = {
            orderId: -1,
            assets: []
        };

        $scope.selectedAssetId = -1;
        $scope.selectedAsset = null;
        $scope.removeAsset = removeAsset;
        $scope.addSelectedAsset = addSelectedAsset;
        $scope.sendDelivery = sendDelivery;

        $scope.$watch('selectedAssetId', handleSelectedAssetIdChanged);

        function removeAsset(asset) {
            if (null == asset) return;
            moveAsset(asset, $scope.newDelivery.assets, $scope.assetsAvailable);
        };

        function addSelectedAsset() {
            var asset = $scope.selectedAsset;

            if (null == asset) return;
            moveAsset(asset, $scope.assetsAvailable, $scope.newDelivery.assets);
        };

        function handleSelectedAssetIdChanged(newValue, oldValue) {
            var newAsset = null;
            for (var idx = 0; idx < $scope.assetsAvailable.length; ++idx) {
                var asset = $scope.assetsAvailable[idx];
                if (asset.id == $scope.selectedAssetId) {
                    newAsset = asset;
                    break;
                }
            }

            $scope.selectedAsset = newAsset;
        };

        function sendDelivery() {
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

    NewCollectionController.$inject = ['$scope', '$http', 'assetsWithCustomers', 'addSuccessCallback'];
    function NewCollectionController($scope, $http, assetsWithCustomers, addSuccessCallback) {
        $scope.assetsWithCustomers = assetsWithCustomers;

        $scope.newCollection = {
            customerId: -1,
            assets: []
        };

        $scope.selectedAssetId = -1;
        $scope.selectedAsset = null;
        $scope.removeAsset = removeAsset;
        $scope.addSelectedAsset = addSelectedAsset;
        $scope.sendCollection = sendCollection;

        $scope.$watch('selectedAssetId', handleSelectedAssetIdChanged);

        function removeAsset(asset) {
            if (null == asset) return;
            moveAsset(asset, $scope.newCollection.assets, $scope.assetsWithCustomers);
        };

        function addSelectedAsset() {
            var asset = $scope.selectedAsset;

            if (null == asset) return;
            moveAsset(asset, $scope.assetsWithCustomers, $scope.newCollection.assets);
        };

        function handleSelectedAssetIdChanged(newValue, oldValue) {
            var newAsset = null;
            for (var idx = 0; idx < $scope.assetsWithCustomers.length; ++idx) {
                var asset = $scope.assetsWithCustomers[idx];
                if (asset.id == $scope.selectedAssetId) {
                    newAsset = asset;
                    break;
                }
            }

            $scope.selectedAsset = newAsset;
        };

        function sendCollection() {
            $http.post('/api/collections/', $scope.newCollection)
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
})();