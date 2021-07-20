// try to change artist-list component to controller for DevExpress
(function () {
    'use strict';

    angular
        .module('clientApp')
        .controller('artistListController', artistListController);

    artistListController.$inject = ['$scope', 'artistService'];

    function artistListController($scope, artistService) {
        $scope.title = 'artistList';    

        $scope.artists = [];

        $scope.loadArtists = function () {
            artistService.getList().then(
                // on success, set state to received data
                function (res) {
                    console.log(res.data);
                    $scope.artists = res.data;
                    //$scope.dataGridOptions.dataSource = self.artists;
                    //console.log($scope.dataGridOptions);
                },
                // on fail, just console.log
                function () {
                    console.log('Error receiving data')
                }
            );
        }

        $scope.dataGridOptions = {
            dataSource: [{ "Id": 1, "Name": "Daniel", "Description": "Generally a good person" }],
            keyExpr: "Id",
            columnMinWidth: 50,
            columnAutoWidth: true,
            columns: ["Name", "Description"]
        }

        $scope.loadArtists();

        activate();

        function activate() { }
    }
})();
