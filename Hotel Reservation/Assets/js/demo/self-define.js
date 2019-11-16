// Set singular or plural units for Number of adult and child
function PluralDetect(adult, child) {
    $(adult).each(function () {
        let adultNum = parseInt($(this).text())
        if (adultNum > 1) {
            $(this).append('Adults')
            console.log('adults')
        } else {
            $(this).append('Adult')
            console.log('adult')
        }
        console.log(this)
    })

    $(child).each(function () {
        let childNum = parseInt($(this).text())
        if (childNum > 1) {
            $(this).append('Children')
            console.log('children')
        } else {
            $(this).append('Child')
            console.log('child')
        }
    })
}

// Trigger button when date chage in Select Room
function onDateChange(dateElement, fakeDateElement) {
    $(dateElement).change(function () {
        $('.booking-wrapper a:last-of-type').removeAttr('hidden')
        $('.booking-wrapper a:first-of-type').attr('hidden', true)
        $(fakeDateElement).attr('value', $(dateElement).val())
        console.log($(dateElement).val())
    })
}

// Update date in Select Room
function onDateSubmit() {
    $('.booking-wrapper a:last-of-type').click(function (e) {
        e.preventDefault();
        var form = $('#updateDate')
        form.submit()
    })
}
function submitForm() {
    $('.booking-wrapper a:first-of-type').click(function (e) {
        e.preventDefault();
        var form = $(this).closest('form')
        form.submit()
    })
}