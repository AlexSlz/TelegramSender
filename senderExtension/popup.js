chrome.storage.local.get('contacts', function (result) {
  loadData()
  if (result.contacts != undefined)
    result.contacts.contact.forEach((item) => {
      addContactTolist(item.fullName, item.id, item.AccessHash)
    })
})
chrome.storage.local.get('channels', function (result) {
  if (result.channels != undefined)
    result.channels.channel.forEach((item) => {
      addContactTolist(item.title, item.id, item.AccessHash)
    })
})

function loadData() {
  chrome.storage.local.get('selectedTG', function (result) {
    document.getElementById('tgListBox').value = result.selectedTG
  })
  chrome.storage.local.get('phoneNumber', function (result) {
    if (result.phoneNumber != undefined)
      document.getElementById('mobile-phone').value = result.phoneNumber
  })
  chrome.storage.local.get('ContactTextBox', function (result) {
    if (result.ContactTextBox != undefined)
      document.getElementById('nameContact-input').value = result.ContactTextBox
  })
  chrome.storage.local.get('ChannelTextBox', function (result) {
    if (result.ChannelTextBox != undefined)
      document.getElementById('nameChannel-input').value = result.ChannelTextBox
  })

  chrome.storage.local.get('protocol', function (result) {
    if (result.protocol == undefined) {
      alert('install protocol(open tgSender)')
      checkProtocol()
    }
  })
}

function addContactTolist(name, id, AccessHash) {
  let opt = document.createElement('option')
  document.getElementById('tgListBox').options.add(opt)
  opt.text = name
  opt.value = id
}

document.getElementById('tgListBox').addEventListener('change', () => {
  let selected = document.getElementById('tgListBox').value
  //selectKey
  chrome.storage.local.set({ selectedTG: selected })
})
document.getElementById('remove').addEventListener('click', () => {
  chrome.storage.local.clear(() => {
    console.log('Everything was removed')
  })
})
document.getElementById('getAction').addEventListener('click', () => {
  let phone = document.getElementById('mobile-phone').value
  let req = `tgsender://auth=${toAscii(phone)}`
  if (document.getElementById('getContactCB').checked) {
    req += `/getcontact`
    if (document.getElementById('nameContact-input').value != '') {
      req += `=${toAscii(document.getElementById('nameContact-input').value)}`
    }
  }
  if (document.getElementById('getChannelCB').checked) {
    req += `/getchannel`
    if (document.getElementById('nameChannel-input').value != '') {
      req += `=${toAscii(document.getElementById('nameChannel-input').value)}`
    }
  }
  window.open(req, '_blank')
  sendMessageToBackGround('getContact')
  chrome.storage.local.set({ phoneNumber: phone })
  chrome.storage.local.set({
    ContactTextBox: document.getElementById('nameContact-input').value,
  })
  chrome.storage.local.set({
    ChannelTextBox: document.getElementById('nameChannel-input').value,
  })
})

document.getElementById('deleteRegEdit').addEventListener('click', () => {
  let req = `tgsender://delete`
  chrome.storage.local.remove('protocol')
  window.open(req, '_blank')
})

function sendMessage(message) {
  chrome.tabs.query({ active: true, currentWindow: true }, (tabs) => {
    chrome.tabs.sendMessage(tabs[0].id, message)
  })
}
function sendMessageToBackGround(message) {
  chrome.runtime.sendMessage(message)
}
function toAscii(s) {
  let text = ''
  s.split('').forEach((item) => {
    text += '$' + item.charCodeAt(0)
  })
  return text
}

let checkProtocol = () => {
  let req = `tgsender://checkproto=test`
  sendMessageToBackGround('checkProtocol')
  window.open(req, '_blank')
}
