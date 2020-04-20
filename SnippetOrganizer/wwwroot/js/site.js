$(document).ready(function () {

   $('body').off('show.bs.modal', '#aboutModal');
   $('body').on('show.bs.modal', '#aboutModal', function (e) {

      var button = $(e.relatedTarget);
      var modal = $(this);
      modal.find('.modal-body').load(button.data("remote"));

   });

   //search bar functions - Gist
   $('body').off('keypress', '#gistSearch');
   $('body').on('keypress', '#gistSearch', function (e) {
      return IsAlphaNumeric(e);
   });

   $('body').off('keyup', '#gistSearch');
   $('body').on('keyup', '#gistSearch', function (e) {
      filterGists('gistSearch','ulGists');
   });

   $('body').off('paste', '#gistSearch');
   $('body').on('paste', '#gistSearch', function (e) {
      var element = this;
      setTimeout(function () {
         var text = $(element).val();
         $(element).val(text.replace(/[^ a-zA-Z0-9]/g, ''));
      }, 100);
   });

   //search bar functions - Repo
   $('body').off('keypress', '#repoSearch');
   $('body').on('keypress', '#repoSearch', function (e) {
      return IsAlphaNumeric(e);
   });

   $('body').off('keyup', '#repoSearch');
   $('body').on('keyup', '#repoSearch', function (e) {
      filterGists('repoSearch', 'ulRepos');
   });

   $('body').off('paste', '#repoSearch');
   $('body').on('paste', '#repoSearch', function (e) {
      var element = this;
      setTimeout(function () {
         var text = $(element).val();
         $(element).val(text.replace(/[^ a-zA-Z0-9]/g, ''));
      }, 100);
   });

}); 

var specialKeys = new Array();
specialKeys.push(8); //Backspace
specialKeys.push(9); //Tab
specialKeys.push(46); //Delete
specialKeys.push(36); //Home
specialKeys.push(35); //End
specialKeys.push(37); //Left
specialKeys.push(39); //Right
specialKeys.push(32); //(Space)

function IsAlphaNumeric(e) {
   var keyCode = e.keyCode === 0 ? e.charCode : e.keyCode;
   var ret = ((keyCode === 32) ||             // (Space)
      (keyCode >= 48 && keyCode <= 57) ||   // numeric (0-9)
      (keyCode >= 65 && keyCode <= 90) ||   // upper alpha (A-Z)
      (keyCode >= 97 && keyCode <= 122) ||  // lower alpha (a-z)
      (specialKeys.indexOf(e.keyCode) !== -1 && e.charCode !== e.keyCode));
   return ret;
}


function filterGists(searchId, ulId) {
   var input, filter, ul, li, li2, a, i;
   input = document.getElementById(searchId);
   filter = input.value.toUpperCase();

   var partial = '';
   var list = filter.split(' ');

   for (i = 0; i < list.length; i++) {
      if (list[i] !== '') {
         var pre = partial;
         var word = '(?=.*\\b'.concat(list[i], '\\w*\\b)');
         partial = pre.concat(word);
      }
   };

   var pattern = partial.concat('.*');
   var regEx = new RegExp(pattern, 'i');

   ul = document.getElementById(ulId);
   li = ul.getElementsByTagName('li');
   for (i = 0; i < li.length; i++) {

      var cls = li[i].classList;
      if (cls.contains('d-none')) {
         cls.remove('d-none')
      }

      var testData = li[i].textContent;
      testData = testData.replace(/(\W)/gm, ' x ');
      if (!regEx.test(testData)) {
         cls.add('d-none');
      }
   };
}
