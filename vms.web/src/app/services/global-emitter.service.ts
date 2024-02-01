import { Injectable,EventEmitter } from '@angular/core';

@Injectable()
export class GlobalEmitterService {
  emitLibraryContent: EventEmitter<any> 
  emitRefreshToken: EventEmitter<any> 
  emitlibraryBatchEditData: EventEmitter<any>
  emitUserData:EventEmitter<any> 
  emitNewUserData:EventEmitter<any> 
  emitPatyScreen:EventEmitter<any>
  emitdeletedUserData:EventEmitter<any> 
  emitData:EventEmitter<any> 
  emitMatterParty:EventEmitter<any>
  emitUserInfo:EventEmitter<any> 
  emitPaymentInfo:EventEmitter<any> 
  emitDeletedPaymentData:EventEmitter<any> 
  emitLicenseList:EventEmitter<any> 
  emitEditedUser:EventEmitter<any>
  emitNewPayment:EventEmitter<any>
  emitAllPractice:EventEmitter<any>
  emitPractice:EventEmitter<any> 
  emitEditedPractice:EventEmitter<any> 
  emitPracticeData:EventEmitter<any> 
  emitDeletedPractice:EventEmitter<any> 
  emitNewRole:EventEmitter<any> 
  emitEditedRole:EventEmitter<any> 
  emitUnitBarData:EventEmitter<any> 
  emitGlobalFields:EventEmitter<any> 
  emitSubmitClicked:EventEmitter<any> 
  emitTaskPanel:EventEmitter<any> 
  emitGlobalField:EventEmitter<any> 
  emitParameter:EventEmitter<any> 
  emitPreviewTemplate:EventEmitter<any> 
  emitCompanyChange:EventEmitter<any>;
  emitNewHtmlAddedToLibrary:EventEmitter<any>;
  emitPanelOneBViewData:EventEmitter<any>;
  constructor() {
    this.emitLibraryContent=new EventEmitter<any>();
    this.emitlibraryBatchEditData=new EventEmitter<any>();
    this.emitUserData = new EventEmitter<any>();
    this.emitNewUserData = new EventEmitter<any>();
    this.emitPatyScreen=new EventEmitter<any>();
    this.emitdeletedUserData=new EventEmitter<any>();
    this.emitData=new EventEmitter<any>();
    this.emitMatterParty=new EventEmitter<any>();
    this.emitUserInfo=new EventEmitter<any>();
    this.emitPaymentInfo=new EventEmitter<any>();
    this.emitDeletedPaymentData=new EventEmitter<any>();
    this.emitLicenseList=new EventEmitter<any>();
    this.emitEditedUser=new EventEmitter<any>();
    this.emitNewPayment=new EventEmitter<any>();
    this.emitAllPractice=new EventEmitter<any>();
    this.emitPractice=new EventEmitter<any>();
    this.emitEditedPractice=new EventEmitter<any>();
    this.emitPracticeData=new EventEmitter<any>();
    this.emitDeletedPractice=new EventEmitter<any>();
    this.emitNewRole=new EventEmitter<any>();
    this.emitEditedRole=new EventEmitter<any>();
    this.emitUnitBarData = new EventEmitter<any>();   
    this.emitGlobalFields = new EventEmitter<any>();   
    this.emitSubmitClicked = new EventEmitter<any>();  
    this.emitTaskPanel= new EventEmitter<any>();  
    this.emitGlobalField= new EventEmitter<any>();  
    this.emitParameter = new EventEmitter<any>();
    this.emitPreviewTemplate = new EventEmitter<any>();
    this.emitRefreshToken = new EventEmitter<any>();
    this.emitCompanyChange = new EventEmitter<any>();
    this.emitNewHtmlAddedToLibrary = new EventEmitter<any>(); 
    this.emitPanelOneBViewData = new EventEmitter<any>(); 
   }
sendSelectedLibraryContent(content){
  this.emitLibraryContent.emit(content);
};
sendNewHtmlAddedToLibrary(data) {
  this.emitNewHtmlAddedToLibrary.emit(data);
}
sendLibraryBatchEditData(data){
  this.emitlibraryBatchEditData.emit(data);
};
sendUserData(data){
  this.emitUserData.emit(data);
};
getUserData(data){
  this.emitNewUserData.emit(data);
};
sendPartyScreen(data){
  this.emitPatyScreen.emit(data);
};
sendMatterParty(data){
  this.emitMatterParty.emit(data);
};
deleteUserData(data){
  this.emitdeletedUserData.emit(data);
};
sendData(data){
  this.emitData.emit(data);
};
sendUserInfo(data){
  this.emitUserInfo.emit(data);
};
sendPaymentInfo(data){
  this.emitPaymentInfo.emit(data);
};
deletePaymentData(data){
  this.emitDeletedPaymentData.emit(data);
};

sendLicenseList(data){
  this.emitLicenseList.emit(data);
};

sendEditedUser(data){
  this.emitEditedUser.emit(data);
};

sendNewPayment(data){
  this.emitNewPayment.emit(data);
};

sendAllPractice(data){
  this.emitAllPractice.emit(data);
};

sendNewPractice(data){
  this.emitPractice.emit(data);
};
sendEditedPractice(data){
  this.emitEditedPractice.emit(data);
};

sendPracticeData(data){
  this.emitPracticeData.emit(data);
};

sendDeletedPractice(data){
  this.emitDeletedPractice.emit(data);
};

sendRole(data){
  this.emitNewRole.emit(data);
};
sendEditedRole(data){
  this.emitEditedRole.emit(data);
};

sendUnitBarData(data){
  this.emitUnitBarData.emit(data);
}
sendGlobalFields(data){
  this.emitGlobalFields.emit(data);
}
sendSubmitClicked(data){
  this.emitSubmitClicked.emit(data);
}
notifyTaskPanel(data){
  this.emitTaskPanel.emit(data);
}
sendGlobalField(data){
  this.emitGlobalField.emit(data);
}
sendParameters(data) {
  this.emitParameter.emit(data);
}
sendPreviewTemplate(data) {
  this.emitPreviewTemplate.emit(data);
}
sendRefreshToken(data) {
  this.emitRefreshToken.emit(data);
}
sendPanelOneBViewData(data) {
  this.emitPanelOneBViewData.emit(data);
}
sendCompanyChange(data) {
  this.emitCompanyChange.emit(data);
}
}
