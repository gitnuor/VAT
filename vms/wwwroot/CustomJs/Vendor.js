const genUtil = window.generalUtility;
const listInfo = {
    docs: [],
    docProp: {
        documentInfoId: 'documentInfoId'
    }
}
const classesToShowHideInEvents = {
    nonForeign: 'non-foreign',
    foreign: 'foreign'
}

classesToShowHideInEvents.vendorRelatedList =
    [classesToShowHideInEvents.nonForeign, classesToShowHideInEvents.foreign];

const frmCreate = document.getElementById('frmCreate');

mainFormFields = {
    NationalIdNo: document.getElementById('NationalIdNo'),
    BinNo: document.getElementById('BinNo'),
    isForeignVendor: document.getElementById('IsForeignVendor'),
    isTds: document.getElementById('IsTds'),
    isVds: document.getElementById('IsVds'),
    isAct94: document.getElementById('IsRegisteredUnderActNinetyFour'),
    TdsRate: document.getElementById('Tdsrate'),
    VdsRate: document.getElementById('Vdsrate'),
    Act94No: document.getElementById('RegistrationNumberUnderActNinetyFour')
}

documentField = {
    drpDocumentTypeId: document.getElementById('DocumentType'),
    fileUploadedFile: document.getElementById('FileUpload'),
    txtDocumentRemarks: document.getElementById('DocumentRemarks')
}

btn = {
    btnAddProduct: document.getElementById('btnAddProduct'),
    btnResetProduct: document.getElementById('btnResetProduct'),
    btnAddPayment: document.getElementById('btnAddPayment'),
    btnAddDocument: document.getElementById('btnAddDocument'),
    btnSave: document.getElementById('btnSave'),
    btnFinalSave: document.getElementById('btnFinalSave'),
    btnSaveDraft: document.getElementById('btnSaveDraft'),
    btnResetForm: document.getElementById('btnResetForm')
}

listTables = {
    product: document.getElementById('productTable'),
    productBody: document.getElementById('productTableBody'),
    payment: document.getElementById('paymentTable'),
    paymentBody: document.getElementById('paymentTableBody'),
    doc: document.getElementById('docTable'),
    docBody: document.getElementById('docTableBody')
}

const dropDownFormFields = {
    ddlCountry: document.getElementById('CountryId'),
    ddlDivision: document.getElementById('DivisionOrStateId'),
    ddlDistrict: document.getElementById('DistrictOrCityId'),
    ddlBankBranchCountry: document.getElementById('BankBranchCountryId'),
    ddlBankBranchDistrict: document.getElementById('BankBranchDistrictOrCityId')
}

const commonChangeEventsForDisplay = {
    hideVendorSpecialOptions: () => {
        classesToShowHideInEvents.vendorRelatedList.forEach(element => {
            window.generalUtility.displayOption.hideItemByClassName(element);
        });
    },
    displayForeign: () => {
        commonChangeEventsForDisplay.hideVendorSpecialOptions();
        window.generalUtility.displayOption.displayItemByClassName(classesToShowHideInEvents.foreign);
    },

    displayNonForeign: () => {
        commonChangeEventsForDisplay.hideVendorSpecialOptions();
        window.generalUtility.displayOption.displayItemByClassName(classesToShowHideInEvents.nonForeign);
    }
}

const applyReadOnlyAtribute = {
    setReadonlyAttribute: (element) => {
        window.generalUtility.alterAttr.setAttr(element, "readonly", "readonly");
    },
    removeReadonlyAttribute: (element) => {
        window.generalUtility.alterAttr.removeAttr(element, "readonly");
    }
}

const checkboxStatusCheck = {
    checkingElement: (checkElement, applyElement) => {
        if (checkElement.checked) {
            applyReadOnlyAtribute.removeReadonlyAttribute(applyElement);
        } else {
            applyReadOnlyAtribute.setReadonlyAttribute(applyElement);
        }
    }
}

const DropDownListMaker = (toId, name, routeId, url) => {
    $(toId).val = '';
    var s = '<option value="">' + name + '</option>';
    var link = location.origin + url + routeId;
    $(toId).html(s);
    $.ajax({
        type: "GET",
        url: link,
        data: "{}",
        cache: false,
        success: function(data) {
            s = '<option value="">' + name + '</option>';
            for (var i = 0; i < data.length; i++) {
                s += '<option value="' +
                    data[i].value +
                    '">' +
                    data[i].text +
                    '</option>';
            }
            $(toId).html(s);
        }
    });

}

dropDownFormFields.ddlCountry.addEventListener('change',
    () => {
        const countryId = dropDownFormFields.ddlCountry.value;
        DropDownListMaker(dropDownFormFields.ddlDivision,
            'Select Division',
            parseInt(countryId),
            '/DivisionOrState/GetDivisionsByCountryId/');
    });

dropDownFormFields.ddlDivision.addEventListener('change',
    () => {
        const divisionId = dropDownFormFields.ddlDivision.value;
        DropDownListMaker(dropDownFormFields.ddlDistrict,
            'Select District',
            parseInt(divisionId),
            '/DistrictOrCity/GetDistrictsByDivisionId/');
    });

dropDownFormFields.ddlBankBranchCountry.addEventListener('change',
    () => {
        const countryId = dropDownFormFields.ddlBankBranchCountry.value;
        DropDownListMaker(dropDownFormFields.ddlBankBranchDistrict,
            'Select District',
            parseInt(countryId),
            '/DistrictOrCity/GetDistrictsByCountryId/');
    });

window.addEventListener('load',
    () => {
        checkboxStatusCheck.checkingElement(mainFormFields.isTds, mainFormFields.TdsRate);
        checkboxStatusCheck.checkingElement(mainFormFields.isVds, mainFormFields.VdsRate);
        checkboxStatusCheck.checkingElement(mainFormFields.isAct94, mainFormFields.Act94No);
    });

mainFormFields.isForeignVendor.addEventListener('change',
    () => {
        if (mainFormFields.isForeignVendor.checked) {
            commonChangeEventsForDisplay.displayForeign();
        } else {
            commonChangeEventsForDisplay.displayNonForeign();
        }
    });

mainFormFields.isTds.addEventListener('change',
    () => {
        checkboxStatusCheck.checkingElement(mainFormFields.isTds, mainFormFields.TdsRate);
    });

mainFormFields.isVds.addEventListener('change',
    () => {
        checkboxStatusCheck.checkingElement(mainFormFields.isVds, mainFormFields.VdsRate);
    });

mainFormFields.isAct94.addEventListener('change',
    () => {
        checkboxStatusCheck.checkingElement(mainFormFields.isAct94, mainFormFields.Act94No);
    });

frmCreate.addEventListener('submit',
    (event) => {
        if (!mainFormFields.isForeignVendor.checked &&
            mainFormFields.NationalIdNo.value === '' &&
            mainFormFields.BinNo.value === '') {
            event.preventDefault();
            window.generalUtility.message.showErrorMsg('Either NID or BIN is required!');
        }
    });


//Add Document Local to Cart
const addRelatedFiles = () => {
    const uploadedFiles = documentField.fileUploadedFile.files;
    const docRemarks = documentField.txtDocumentRemarks.value;
    const documentTypeId = documentField.drpDocumentTypeId.value;
    const documentTypeName =
        genUtil.getDataAttributeValue.dropdownSelectedText(documentField.drpDocumentTypeId);
    for (let i = 0; i < uploadedFiles.length; i++) {
        const documentInfo = {
            documentInfoId: genUtil.getRandomString(6),
            documentTypeId: documentTypeId,
            documentTypeName: documentTypeName,
            uploadedDocumentName: uploadedFiles[i].name,
            uploadedDocument: uploadedFiles[i],
            documentRemarks: docRemarks
        }

        listInfo.docs.push(documentInfo);
        const dataRow = `
            <tr id="tr_${documentInfo.documentInfoId}">
                <td>
                    ${documentInfo.documentTypeName}
                </td>
                <td>
                    ${documentInfo.uploadedDocumentName}
                </td>
                <td>
                    ${documentInfo.documentRemarks}
                </td>
                <td>
                    <button class="btn btn-danger btn-sm" onclick="commonUtilitySale.removeDoc('${documentInfo
            .documentInfoId}');" ><i class="bi bi-trash"></i></button>
                </td>
            </tr>
        `;
        listTables.docBody.insertAdjacentHTML('beforeend', dataRow);
    }
    //    commonChangeEventsForCalculation.resetForm.doc();
}

//Add Document Local to Cart Method Call
btn.btnAddDocument.addEventListener('click',
    () => {
        //console.log('Test');
        //        if (genUtil.validateElement.formByForm(elementInformation.formInfo.frmDocument)) {
        addRelatedFiles();
        //        }
    });