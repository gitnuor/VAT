const genUtil = window.generalUtility;
const listInfo = {
    docs: [],
    docProp: {
        documentInfoId: 'documentInfoId'
    }
}

const specialClassForEvent = {
    branchToShowHide: 'branch-information',
    branchSelectOption: 'is-branch-selected'
}

const formInfo = {
    frmCreate: document.getElementById('frmCreate'),
    frmDocument: document.getElementById('frmDocument')
}

mainFormFields = {
    NationalIdNo: document.getElementById('Nidno'),
    BinNo: document.getElementById('Bin'),
    isRequireBranch: document.getElementById('IsRequireBranch'),
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
    btnAddDocument: document.getElementById('btnAddDocument'),
    btnSave: document.getElementById('btnSave'),
    btnResetForm: document.getElementById('btnReset')
}

listTables = {
    doc: document.getElementById('docTable'),
    docBody: document.getElementById('docTableBody')
}

const commonChangeEventsForDisplay = {
    hideBranch: () => {
        genUtil.displayOption.hideItemByClassName(specialClassForEvent.branchToShowHide);
    },

    displayBranch: () => {
        genUtil.displayOption.displayItemByClassName(specialClassForEvent.branchToShowHide);
    }
}

const applyReadOnlyAttribute = {
    setReadonlyAttribute: (element) => {
        genUtil.alterAttr.setAttr(element, "readonly", "readonly");
    },
    removeReadonlyAttribute: (element) => {
        genUtil.alterAttr.removeAttr(element, "readonly");
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





//Common Utility for Customer
const commonUtilityCustomer = {
    isBinOrNidValid() {
        if (mainFormFields.NationalIdNo.value === '' &&
            mainFormFields.BinNo.value === '') {
            genUtil.message.showErrorMsg('Either NID or BIN is required!');
            return false;
        }
        return true;
    },
    isBranchValid() {
        if (document.querySelectorAll(`.${specialClassForEvent.branchSelectOption}:checked`).length < 1 && mainFormFields.isRequireBranch.checked) {
            genUtil.message.showErrorMsg('When branch is required you must have to check at least one branch!');
            return false;
        }
        return true;
    },
    isInformationValid() {
        return genUtil.validateElement.formByForm(formInfo.frmCreate) & this.isBinOrNidValid() & this.isBranchValid();
    },
    getFormData() {
        const formData = new FormData(formInfo.frmCreate);
        let docIndex = 0;
        listInfo.docs.forEach(d => {
            formData.append(`Documents[${docIndex}].DocumentTypeId`, d.documentTypeId);
            formData.append(`Documents[${docIndex}].UploadedFile`, d.uploadedDocument);
            formData.append(`Documents[${docIndex}].DocumentRemarks`, d.documentRemarks);
            docIndex++;
        });
        return formData;
    },
    postFormData(data) {
        window.$.ajax({
            url: 'Create',
            data: data,
            cache: false,
            method: "POST",
            contentType: false,
            processData: false,
            dataType: 'json',
            success: function (d) {
                if (d.isSaved)
                    window.location.href = `Index`;

                genUtil.message.showErrorMsg(d.message);
                generalUtility.alterAttr.removeAttr(btn.btnSave, 'disabled');
            },
            error: function (d) {
                genUtil.message.showErrorMsg(d);
                generalUtility.alterAttr.removeAttr(btn.btnSave, 'disabled');
            }
        });
    },
    save() {
        if (this.isInformationValid()) {
            this.postFormData(this.getFormData());
        }
    }
}

//#region Event Listener

window.addEventListener('load',
    () => {
        checkboxStatusCheck.checkingElement(mainFormFields.isTds, mainFormFields.TdsRate);
        checkboxStatusCheck.checkingElement(mainFormFields.isVds, mainFormFields.VdsRate);
        checkboxStatusCheck.checkingElement(mainFormFields.isAct94, mainFormFields.Act94No);
    });

btn.btnAddDocument.addEventListener('click',
    () => {
        if (genUtil.validateElement.formByForm(formInfo.frmDocument)) {
            addRelatedFiles();
        }
    });

mainFormFields.isRequireBranch.addEventListener('change',
    () => {
        if (mainFormFields.isRequireBranch.checked) {
            commonChangeEventsForDisplay.displayBranch();
        } else {
            commonChangeEventsForDisplay.hideBranch();
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

formInfo.frmCreate.addEventListener('submit',
    (event) => {
        if (mainFormFields.NationalIdNo.value === '' &&
            mainFormFields.BinNo.value === '') {
            event.preventDefault();
            genUtil.message.showErrorMsg('Either NID or BIN is required!');
        }
    });

btn.btnSave.addEventListener('click',
    () => {
        commonUtilityCustomer.save();
    });

//#endregion



