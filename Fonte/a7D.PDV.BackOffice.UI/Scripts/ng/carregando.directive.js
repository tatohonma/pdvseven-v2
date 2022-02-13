angular.module("backoffice")
  .directive('carregando', carregando);

function carregando() {
  return {
    restrict: "E",
    template: '<div class="col-md-offset-4 col-md-4">' + 
      '<div class="panel panel-default">' +
            '<div class="panel-heading">Carregando informações...</div>' +
            '<div class="panel-body">' +
                '<div class="progress">' +
                    '<div class="progress-bar progress-bar-primary progress-bar-striped active" aria-valuenow="100" style="width: 100%" aria-valuemin="0" aria-valuemax="100">' +
                        '<span class="sr-only">100%</span>' +
                    '</div>' +
                '</div>' +
            '</div>' +
        '</div>' + 
        '</div>'
  };
}