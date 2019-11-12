// Set singular or plural units for Number of adult and child
function PluralDetect(adult, child) {
    $(adult).each(function () {
        let adultNum = parseInt($(this).text())
        if (adultNum > 1) {
            //let adultId = "adultLbl" + adultIdCount.toString()
            //let adultLabel = $(`<label id=${adultId}></label>`).text('Adults')
            //$(this).after(adultLabel)
            $(this).append('Adults')
            console.log('adults')
        } else {
            //let adultId = "adultLbl" + adultIdCount.toString()
            //let adultLabel = $(`<label id=${adultId}></label>`).text('Adult')
            //$(this).after(adultLabel)
            $(this).append('Adult')
            console.log('adult')
        }
        console.log(this)
    })

    $(child).each(function () {
        let childNum = parseInt($(this).text())
        if (childNum > 1) {
            //let childId = "childLbl" + childIdCount.toString()
            //let childLabel = $(`<label id=${childId}></label>`).text('Children')
            //$(this).after(childLabel)
            $(this).append('Children')
            console.log('children')
        } else {
            //let childId = "adultLbl" + childIdCount.toString()
            //let childLabel = $(`<label id=${childId}></label>`).text('Child')
            //$(this).after(childLabel)
            $(this).append('Child')
            console.log('child')
        }
    })
}

// Add new room to booking list
function addNewBookingRoom() {

}