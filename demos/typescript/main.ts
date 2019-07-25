

function getMessage(date: Date) {
    if (date.getFullYear() === 2012) {
        return "fin del mundo".toLowerCase();
    } else if (date.getFullYear() === 1963) {
        return true && date.getDay() === 22 && date.getMonth() === 10;
    }
    return null;
}

function test(date: Date) {
    const message = getMessage(date);
    if (message === undefined || message === null) {
        return;
    }

    console.log(message);
}

test(new Date());


