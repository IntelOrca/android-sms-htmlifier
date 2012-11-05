//https://github.com/azer/relative-date
var relativeDate=(function(undefined){var SECOND=1000,MINUTE=60*SECOND,HOUR=60*MINUTE,DAY=24*HOUR,WEEK=7*DAY,YEAR=DAY*365,MONTH=YEAR/12;var formats=[[0.7*MINUTE,'just now'],[1.5*MINUTE,'a minute ago'],[60*MINUTE,'minutes ago',MINUTE],[1.5*HOUR,'an hour ago'],[DAY,'hours ago',HOUR],[2*DAY,'yesterday'],[7*DAY,'days ago',DAY],[1.5*WEEK,'a week ago'],[MONTH,'weeks ago',WEEK],[1.5*MONTH,'a month ago'],[YEAR,'months ago',MONTH],[1.5*YEAR,'a year ago'],[Number.MAX_VALUE,'years ago',YEAR]];function relativeDate(input,reference){!reference && (reference=(new Date).getTime());reference instanceof Date && (reference=reference.getTime());input instanceof Date &&(input=input.getTime());var delta=reference-input,format,i,len;for(i=-1,len=formats.length;++i<len;){format=formats[i];if(delta<format[0]){return format[2]==undefined?format[1]:Math.round(delta/format[2])+' '+format[1];}};}return relativeDate;})();

(function init(){

  var peopleEl=document.querySelector('#people');
  var sidebarItems=document.getElementById('convo_list').querySelectorAll('a');  
  var timeNodes = document.querySelectorAll('time');

  for (var i = 0, l = timeNodes.length; i < l; i++) {
    var d = new Date ( parseInt ( timeNodes[i].getAttribute('datetime') ) );
    var time = d.toLocaleTimeString ()
    d = d.toLocaleDateString () + ' @ ' + time;
    timeNodes[i].setAttribute('title', d);
    timeNodes[i].innerHTML = relativeDate(timeNodes[i].innerHTML) + ' @ ' + time;
  }

  function sidebarItemClick(e){
    // ...
    console.log('Clicked item with hash of '+e.target.hash);
    // .className = 'convo active';
	var convos = document.querySelectorAll('.convo');
	for (var i = 0, l = convos.length; i < l; i++)
		convos[i].style.display = 'none';
	document.getElementById("convo_" + e.target.hash.substring(1)).style.display = 'block';
    e.preventDefault();
    return false;
  }
  
  for(i=0;i<sidebarItems.length;i++){
    sidebarItems[i].addEventListener("click",sidebarItemClick,false);
  }

  //sidebarItems[0].click();
  document.querySelectorAll('.convo')[0].style.display='block';

  peopleEl.innerHTML=people[0];

})();