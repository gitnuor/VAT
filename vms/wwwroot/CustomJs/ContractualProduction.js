const logicalInformation = {
    isInvalidQty: false
}

const listInfo = {
    rawMaterial: [],
}

formInfo = {
    selfproductionFrm: document.getElementById('SelfproductionFrm'),
    documentFrm: document.getElementById('documentFrm'),
}


const mainFormFields = {
    txtBatchNo: document.getElementById('BatchNo'),
    drpProductId: document.getElementById('ProductId'),
    txtReceiveQuantity: document.getElementById('ReceiveQuantity'),
    txtMeasurementName: document.getElementById('MeasurementName'),
    dateReceiveTime: document.getElementById('ReceiveTime'),
    drpContractualProductionId: document.getElementById('ContractualProductionId'),
    txtChallanNo: document.getElementById('ChallanNo'),
    hdmeasurementUnitId: document.getElementById('MeasurementUnitId'),
}

const dataAttributes = {
    product: {
        measurementUnitId: 'data-product-measurement-unit-id',
        measurementUnitName: 'data-product-measurement-unit-name'
    }
}

const contentTableField = {
    drpDocumentType: document.getElementsByClassName('DocumentType'),
    fileFileUpload: document.getElementsByClassName('FileUpload'),
}

mainFormFields.drpProductId.addEventListener('change', (event) => {
    const drp = event.target;
    mainFormFields.hdmeasurementUnitId.value = window.generalUtility.getDataAttributeValue.dropdownSelected(drp, dataAttributes.product.measurementUnitId);
    mainFormFields.txtMeasurementName.value = window.generalUtility.getDataAttributeValue.dropdownSelected(drp, dataAttributes.product.measurementUnitName);
    mainFormFields.txtReceiveQuantity.value = 1;
    getRawMaterial(drp.value);
});

mainFormFields.txtReceiveQuantity.addEventListener('input', (event) => {
    const receiveQuantity = event.target.value;
    if (receiveQuantity <= 0) {
        UsedQtyCalculation(0);
    } else {
        UsedQtyCalculation(receiveQuantity);
    }
});
const fileAddControl = {
    fileDocument: document.getElementById('fileDocument'),
    drpFileType: document.getElementById('drpFileType'),
    tbdAttachedFiles: document.getElementById('tbdAttachedFiles'),
    txtDocumentRemarks: document.getElementById('DocumentRemarks'),
    RelatedDocumentList: []
}

const fileUploadValidationMessage = {
    spnFileType: document.getElementById('msgFileType'),
    spnFileDocument: document.getElementById('msgFileDocument')
}

const btn = {
    btnAddRelatedFile: document.getElementById('btnAddRelatedFile'),
    btnSave: document.getElementById('save'),
}

btn.btnAddRelatedFile.addEventListener('click', () => {
    /* if (isFileTypeValid()) {*/
    if (window.generalUtility.validateElement.formByForm(formInfo.documentFrm)) {
        addRelatedFiles();
        /*        }*/
    }
});

const isFileTypeValid = () => {
    let result = fileTypeValidator.isFileTypeValid();
    result = fileTypeValidator.isFileUploadValid();
    return fileTypeValidator.isFileTypeValid()
        && fileTypeValidator.isFileUploadValid();
}


const vatValidator = {
    required: (ctrl) => {
        if (ctrl.value.length === 0) {
            ctrl.classList.add('vat-error');
            return false;
        }
        ctrl.classList.remove('vat-error');
        return true;
    },
    dropDownRequired: (ctrl) => {
        if (ctrl.selectedIndex === 0) {
            ctrl.classList.add('vat-error');
            return false;
        }
        ctrl.classList.remove('vat-error');
        return true;
    }
}

const fileTypeValidator = {
    isFileTypeValid: () => {
        if (vatValidator.required(fileAddControl.drpFileType)) {
            fileUploadValidationMessage.spnFileType.textContent = "";
            return true;
        }
        else {
            fileUploadValidationMessage.spnFileType.textContent = "File type is required";
        }
        return false;
    },
    isFileUploadValid: () => {
        if (vatValidator.required(fileAddControl.fileDocument)) {
            fileUploadValidationMessage.spnFileDocument.textContent = "";
            return true;
        }
        else {
            fileUploadValidationMessage.spnFileDocument.textContent = "No file added";

        }
        return false;
    }
}

fileAddControl.drpFileType.addEventListener('input', () => {
    fileTypeValidator.isFileTypeValid();
});

const resetForm = {
    doc: () => {
        formInfo.documentFrm.reset();
        window.generalUtility.setDropdownValue.selectPickerReset(fileAddControl.drpFileType);
    }
}

const addRelatedFiles = () => {
    const newFiles = fileAddControl.fileDocument.files;
    for (let i = 0; i < newFiles.length; i++) {
        const newFileName = newFiles[i].name;
        const newFileDocumentTypeName = fileAddControl.drpFileType.options[fileAddControl.drpFileType.selectedIndex].text;
        const newFileRndId = Math.random().toString(36).substring(2);
        const newDocumentInfo = {
            RndId: newFileRndId,
            DocumentTypeId: fileAddControl.drpFileType.value,
            DocumentTypeName: newFileDocumentTypeName,
            DocumentRemarks: fileAddControl.txtDocumentRemarks.value,
            UploadedFile: newFiles[i]
        }
        fileAddControl.RelatedDocumentList.push(newDocumentInfo);
        const newFileDataRow = `
        <tr id="row_${newFileRndId}">
            <td>${newFileDocumentTypeName}</td>
            <td>${newFileName}</td>
            <td>${newDocumentInfo.DocumentRemarks}</td>
            <td><button type="button" class="btn btn-danger btn-sm btn-block btn-remove-related-file" onclick="removeRelatedFile('${newFileRndId}')">Delete</button></td>
        </tr>
        `;
        fileAddControl.tbdAttachedFiles.insertAdjacentHTML('beforeend', newFileDataRow);
    }

    resetForm.doc();
}

const removeRelatedFile = (fileRndId) => {
    const fileDisplayRowId = document.getElementById(`row_${fileRndId}`);
    fileDisplayRowId.remove();
    fileAddControl.RelatedDocumentList = fileAddControl.RelatedDocumentList.filter(f => f.RndId !== fileRndId);
}

const saveData = (data) => {
    //disContentById('loaderAnimation');
    $.ajax({
        url: 'ContractualProduction',
        data: data,
        type: "POST",
        contentType: false,
        processData: false,
        dataType: 'json',
        success: function () {
            //viewSuccessFeedBack();
            //hideContentById('loaderAnimation');
            window.location.href = `/Production`;
        },
        error: function () {
            //viewErrorFeedBack();
            //hideContentById('loaderAnimation');
        }
    });
}


const UsedQtyCalculation = (value) => {
    $("#BomInfo").empty();
    for (var i = 0; i < listInfo.rawMaterial.length; i++) {
        var item = listInfo.rawMaterial[i].requiredQtyPerUnitProduction * value;
        var html = '';
        if (listInfo.rawMaterial[i].currentStock < item) {
            logicalInformation.isInvalidQty = true;
            html = '<tr class="table-danger" id="' +
                listInfo.rawMaterial[i].id +
                '">' +
                '<td>' +
                listInfo.rawMaterial[i].prodName +
                '</td>' +
                '<td>' +
                listInfo.rawMaterial[i].requiredQtyPerUnitProduction +
                '</td>' +
                '<td>' +
                listInfo.rawMaterial[i].currentStock +
                '</td>' +
                '<td>' +
                item +
                '</td>' +
                '<td>' +
                listInfo.rawMaterial[i].unitName +
                '</td>'
            html += '</tr>';
        } else {
            logicalInformation.isInvalidQty = false;
             html = '<tr id="' +
                listInfo.rawMaterial[i].id +
                '">' +
                '<td>' +
                listInfo.rawMaterial[i].prodName +
                '</td>' +
                '<td>' +
                listInfo.rawMaterial[i].requiredQtyPerUnitProduction +
                '</td>' +
                '<td>' +
                listInfo.rawMaterial[i].currentStock +
                '</td>' +
                '<td>' +
                item +
                '</td>' +
                '<td>' +
                listInfo.rawMaterial[i].unitName +
                '</td>'
            html += '</tr>';
        }

        $("table#gridTable > tbody").append(html);
    }
}
const getRawMaterial = (id) => {
    $.ajax({
        type: "GET",
        url: "/Products/ProductionReceiveGetData/",
        data: { prodId: id },
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            //$("#loading").show();
        },

        success: function (result) {
            listInfo.rawMaterial = result;
            UsedQtyCalculation(1);
        },
        error: function (x, e) {
            //$("#loading").hide();
            alert('error');
        }
    });
}


btn.btnSave.addEventListener('click', () => {

    if (logicalInformation.isInvalidQty) {
        toastr.error('Invalid Quantity Please Check Raw BOM List');
        return false;
    }

    if (window.generalUtility.validateElement.formByForm(formInfo.selfproductionFrm)) {
        sendToCaseManager();
    }

});

const sendToCaseManager = () => {
    let formData = new FormData(formInfo.selfproductionFrm);
    let relatedDocIndex = 0;
    formData.append("ContractualProductionId", mainFormFields.drpContractualProductionId.value);
    formData.append("ChallanNo", mainFormFields.txtChallanNo.value);
    formData.append("BatchNo", mainFormFields.txtBatchNo.value);
    formData.append("ProductId", mainFormFields.drpProductId.value);
    formData.append("MeasurementUnitId", mainFormFields.hdmeasurementUnitId.value);
    formData.append("ReceiveQuantity", mainFormFields.txtReceiveQuantity.value);
    formData.append("ReceiveTime", mainFormFields.dateReceiveTime.value);
    for (const relatedDoc of fileAddControl.RelatedDocumentList) {
        formData.append(`ContractualProductionDocumentList[${relatedDocIndex}].DocumentTypeId`, relatedDoc.DocumentTypeId);
        formData.append(`ContractualProductionDocumentList[${relatedDocIndex}].UploadedFile`, relatedDoc.UploadedFile);
        formData.append(`ContractualProductionDocumentList[${relatedDocIndex}].DocumentRemarks`, relatedDoc.DocumentRemarks);
        relatedDocIndex++;
    }
        saveData(formData);

}