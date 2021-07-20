(function () {
    angular.
        module('clientApp').
        component('artistList', {
            
            controller: function artistListController(artistService, $http, $scope) {
                var self = this;
                self.artists = [];
                var store = new DevExpress.data.CustomStore({
                    key: "id",
                    load: function () {
                        return artistService.getList().then(
                            // on success, set state to received data
                            function (res) {
                                console.log('got data', res.data);
                                return {
                                    data: res.data
                                }
                            },
                            // on fail, just console.log
                            function () {
                                console.log('Error receiving data')
                            }
                        );
                    },
                })

                $scope.dataGridOptions = {
                    dataSource: store,
                    remoteOperations: false,
                    columns: [
                        {
                            dataField: "name",
                            dataType: "string"
                        },
                        {
                            dataField: "description",
                            dataType: "string"
                        },
                        {
                            dataField: "facebookLink",
                            dataType: "string"
                        },
                    ],
                    searchPanel: {
                        visible: true,
                        highlightCaseSensitive: true
                    },
                }

                //self.loadArtists();
                setTimeout(() => console.log(store), 5000)
                
            },
            templateUrl: "/components/artist-list/artist-list.template.html" 
        });
})();


