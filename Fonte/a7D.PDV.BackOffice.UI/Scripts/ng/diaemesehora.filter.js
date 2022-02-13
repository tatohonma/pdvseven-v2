angular.module('backoffice')
  .filter('diaemesehora', diaemesehora);

diaemesehora.$inject = ["$window"];

function diaemesehora($window) {
  return function(x) {
    return $window.moment(x).format("DD/MM HH:mm");
  }
}