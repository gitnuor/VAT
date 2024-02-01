const genUtil = window.generalUtility;

const specialClassForEvent = {
    branchToShowHide: 'branch-information',
    branchSelectOption: 'is-branch-selected'
}

const formInfo = {
    frmCreate: document.getElementById('frmusercreate')
}

mainFormFields = {
    isRequireBranch: document.getElementById('IsRequireBranch')
}

const commonChangeEventsForDisplay = {
    hideBranch: () => {
        genUtil.displayOption.hideItemByClassName(specialClassForEvent.branchToShowHide);
    },

    displayBranch: () => {
        genUtil.displayOption.displayItemByClassName(specialClassForEvent.branchToShowHide);
    }
}

const checkboxStatusCheck = {
    checkingElement: (checkElement, applyElement) => {
        if (checkElement.checked) {
            applyReadOnlyAttribute.removeReadonlyAttribute(applyElement);
        } else {
            applyReadOnlyAttribute.setReadonlyAttribute(applyElement);
        }
    }
}

mainFormFields.isRequireBranch.addEventListener('change',
    () => {
        if (mainFormFields.isRequireBranch.checked) {
            commonChangeEventsForDisplay.displayBranch();
        } else {
            commonChangeEventsForDisplay.hideBranch();
        }
    });