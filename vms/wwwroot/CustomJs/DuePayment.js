const genUtil = window.generalUtility;
const elementInformation = {
    paymentField: {
        drpPaymentMethodId: document.getElementById('PaymentMethodId'),
        drpBankId: document.getElementById('bankId'),
        txtPaymentDate: document.getElementById('paymentDate'),
        txtMobilePaymentWalletNo: document.getElementById('mobilePaymentWalletNo'),
        txtPaymentDocumentOrTransDate: document.getElementById('paymentDocumentOrTransDate'),
        txtPaymentRemarks: document.getElementById('paymentRemarks'),
        txtDocumentNoOrTransactionId: document.getElementById('documentNoOrTransactionId'),
        txtPaidAmount: document.getElementById('paidAmount'),
    },
}

//Special Item Id
const specialItem = {
    classesToShowHideInEvents: {
        bankPayment: 'payment-bank',
        mobilePayment: 'payment-mobile',
    }
}


//Payment Hide Show CSS Class List
specialItem.classesToShowHideInEvents.paymentRelatedList =
    [specialItem.classesToShowHideInEvents.bankPayment, specialItem.classesToShowHideInEvents.mobilePayment];

//Call Show Hide Field Depend on PaymentMethod Change
elementInformation.paymentField.drpPaymentMethodId.addEventListener('change', (event) => {
    const isBankingChannel =
        genUtil.getDataAttributeValue.dropdownSelected(event.target, dataAttributes.payment.isBankingChannel);
    const isMobileTransaction =
        genUtil.getDataAttributeValue.dropdownSelected(event.target, dataAttributes.payment.isMobileTransaction);
    if (isBankingChannel === 'True') {
        commonChangeEventsForDisplay.displayPaymentBank();
    } else if (isMobileTransaction === 'True') {
        commonChangeEventsForDisplay.displayPaymentMobile();
    } else {
        commonChangeEventsForDisplay.hidePaymentSpecialOptions();
    }
});


//Data Attribute Id Assign
const dataAttributes = {
    payment: {
        isBankingChannel: 'data-is-banking-channel',
        isMobileTransaction: 'data-is-mobile-transaction'
    }
}

//Common Show or Hide for Change Event
const commonChangeEventsForDisplay = {
    hidePaymentSpecialOptions: () => {
        specialItem.classesToShowHideInEvents.paymentRelatedList.forEach(element => {
            genUtil.displayOption.hideItemByClassName(element);
        });
    },
    displayPaymentBank: () => {
        commonChangeEventsForDisplay.hidePaymentSpecialOptions();
        genUtil.displayOption.displayItemByClassName(specialItem.classesToShowHideInEvents.bankPayment);
    },
    displayPaymentMobile: () => {
        commonChangeEventsForDisplay.hidePaymentSpecialOptions();
        genUtil.displayOption.displayItemByClassName(specialItem.classesToShowHideInEvents.mobilePayment);
    },
}