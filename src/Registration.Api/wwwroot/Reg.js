/// <reference path="https://ajax.googleapis.com/ajax/libs/angularjs/1.3.14/angular.min.js" />


var todoApp = angular.module('RegApp', []);

todoApp.controller('regCtrl', function ($scope, $http) {

    $scope.userDetails = {};
    $scope.regToken = '';
    $scope.loginToken = '';
    $scope.LMToken = '';

    $scope.getRegToken = function () {
        $http.post("api/token", { brand: "1", subBrand: "1" }).success(function (responseData) {
            $scope.regToken = responseData.token;
        });
    }


    $scope.Register = function () {    
        $http.post("api/Registration", $scope.userDetails, {
            headers: { 'Content-Type': 'application/json','Authorization': `Bearer ${$scope.regToken}` },            
        }).success(function(responseData) {
            $scope.loginToken = responseData.LoginAccessToken;
        });
    }

    $scope.Login = function () {    
        $http.get("api/Login",{
            headers: { 'Content-Type': 'application/json','Authorization': `Bearer ${$scope.loginToken}` },            
        }).success(function(responseData) {            
            console.dir(responseData.ActualLoginToken);
            $scope.LMToken = responseData.ActualLoginToken;
        });
    }
    $scope.getRegToken();

});