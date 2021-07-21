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
                    update: function (key, values) {
                        return artistService.update(key, values).then(
                            function (res) {
                                console.log("updated artist", res);
                            },
                            function () {
                                console.log("error updating artist")
                            }
                        );
                    },
                    insert: function (values) {
                        return artistService.create(values).then(
                            function (res) {
                                console.log("created artist", res);
                            },
                            function () {
                                console.log("error creating artist")
                            }
                        );
                    },
                    remove: function (key) {
                        return artistService.delete(key).then(
                            function (res) {
                                console.log("deleted artist", res);
                            },
                            function () {
                                console.log("error deleting artist")
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
                    editing: {
                        mode: "row",
                        allowUpdating: true,
                        allowDeleting: true,
                        allowAdding: true
                    },
                    onSaved: function (e) {
                        console.log(e);
                    }
                }

                //self.loadArtists();
                setTimeout(() => console.log(store), 5000)
                
            },
            templateUrl: "/components/artist-list/artist-list.template.html" 
        });
})();


