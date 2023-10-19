let index = 0;

function AddTag() {

    var tagEntry = $('#TagEntry').val().trim();
    if (tagEntry === '') {
        Swal.fire('Error', 'Tag cannot be empty.', 'error');
        return;
    }

    var tagEntry = document.getElementById("TagEntry");

    //lets use the new search funtion to help detect an error state
    //let searchResult = document.getElementById(tagEntry.value);
    //if (searchResult === null) {
    //    //trigger my sweet alert for whatever condition is contsined in the searchResult var
    //    Swal.fire({
    //        title: 'Error!',
    //        text: 'Tag cannot be empty.',
    //        icon: 'error',
    //        confirmationButtonText:'Ok'
    //    })




    //}
 //   else {
        //create a new selected option
        let newOption = new Option(tagEntry.value, tagEntry.value);
        document.getElementById("TagList").options[index++] = newOption;
        
   // }

    

    tagEntry.value = "";
    return true;
}

function DeleteTag() {
    var selectedTags = $('#TagList option:selected');
    if (selectedTags.length === 0) {
        Swal.fire('Error', 'No tags selected.', 'error');
        return;
    }

    let tagList = document.getElementById("TagList");
    let selectedIndex = tagList.selectedIndex;
    let tagCount = 0;

    while (selectedIndex >= 0) {
        tagList.options[selectedIndex] = null;
        selectedIndex = tagList.selectedIndex;
        tagCount++;
    }

    index -= tagCount;
}

$("form").on("submit", function () {
    $("#TagList option").prop("selected", "selected");
});



if (tagValues != '') {
    let tagArray = tagValues.split(",");

    for (let loop = 0; loop < tagArray.length; loop++) {
        ReplaceTag(tagArray[loop], loop);
        index++;
    }
}

function ReplaceTag(tag, index) {
    let newOption = new Option(tag, tag);
    document.getElementById("TagList").options[index] = newOption;
}

//the search function wil  detect either an empty or a duplicate tag

//and return and errr string of an error is detected

function search(str) {
    if (str == "") {
        return 'Empty tags are not permitted';
    }


    var tagsE1 = document.getElementById('TagList');
    if (tagsE1) {
        let options = tagsE1.options;
        for (let i = 0; index < options.length; index++) {
            if (options[index].value == str) {
                return 'The Tag #${str} was detected as a duplicate and not permitted'
            }
        }
    }
}


//let index = 0;

//function AddTag() {
//    // get th refernece to the tag entry eleent
//    var tagEntry = document.getElementById("TagEntry");


//    let newOption = new Option(tagEntry.value, tagEntry.value);
//    document.getElementById("TagList").options[index++] = newOption;

//    //clear out tag entry control
//    tagEntry.value = "";
//    return true;
//}


//function DeleteTag() {

//    let tagCount = 1;
//    while (tagCount > 0) {
//        let tagList = document.getElementById("TagList");
//        let selectedIndex = tagList.selelctedIndex;

//        if (selectedIndex >= 0) {
//            tagList.options[selectedIndex] = null;
//            --tagCount;
//        }
//        else 
//            tagCount = 0;
//        index--;
//    }

//}



//$("form").on("submit", function (){
//    $("#TagList option").prop("selected", "selected");
//})



//if (tagValues != '') {
//    let tagArray = tagValues.split(",");

//    for (let loop = 0; loop < tagArray.length; loop++) {
//        ReplaceTag(tagArray[loop], loop);
//        index++;
//    }

//}


//function ReplaceTag(tag, index) {
//    let newOption = new Option(tag, tag);
//    document.getElementById("TagList").options[index] = newOptions;
//}