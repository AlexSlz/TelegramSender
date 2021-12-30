chrome.extension.onMessage.addListener(function (message) {
  if (message == 'getContact') {
    connectAndGetMsg((e) => {
      console.log(e.target.result)
      var jsonData = JSON.parse(e.target.result).tgData
      for (var i = 0; i < jsonData.length; i++) {
        switch (Object.keys(JSON.parse(e.target.result).tgData[i])[0]) {
          case 'contact':
            //ContactKey
            chrome.storage.local.set({
              contacts: JSON.parse(e.target.result).tgData[i],
            })
            break
          case 'channel':
            chrome.storage.local.set({
              channels: JSON.parse(e.target.result).tgData[i],
            })
            console.log(JSON.parse(e.target.result).tgData[i])
            break
          default:
            break
        }
      }
      loadData()
    })
  }
})

function connectAndGetMsg(callback) {
  let socket = new WebSocket('ws://localhost:80')
  socket.onmessage = function (msg) {
    let reader = new FileReader()
    reader.onload = callback
    reader.readAsText(msg.data)
    socket.close()
  }
}

var contextMenuImage = {
  'id': 'img',
  'title': 'SendImage',
  'contexts': ['image'],
}
var contextMenuAudio = {
  'id': 'audio',
  'title': 'SendAudio',
  'contexts': ['audio'],
}
var contextMenuVideo = {
  'id': 'video',
  'title': 'SendVideo',
  'contexts': ['video'],
}
var TempImgDownload = {
  'id': 'img-download',
  'parentId': 'img',
  'title': 'JustDownload',
  'contexts': ['image'],
}
var TempVideoDownload = {
  'id': 'video-download',
  'parentId': 'video',
  'title': 'JustDownload',
  'contexts': ['video'],
}

loadData()
function loadData() {
  chrome.contextMenus.removeAll()

  chrome.contextMenus.create(contextMenuImage)
  chrome.contextMenus.create(TempImgDownload)

  chrome.contextMenus.create(contextMenuAudio)

  chrome.contextMenus.create(contextMenuVideo)
  chrome.contextMenus.create(TempVideoDownload)
  chrome.storage.local.get('contacts', function (result) {
    if (result.contacts != undefined) {
      addUser(result.contacts.contact, {
        title: 'fullName',
        parentId: 'img',
        contexts: 'image',
      })
      addUser(result.contacts.contact, {
        title: 'fullName',
        parentId: 'video',
        contexts: 'video',
      })
      addUser(result.contacts.contact, {
        title: 'fullName',
        parentId: 'audio',
        contexts: 'audio',
      })
    }
  })
  chrome.storage.local.get('channels', function (result) {
    if (result.channels != undefined) {
      addUser(result.channels.channel, {
        title: 'title',
        parentId: 'img',
        contexts: 'image',
      })
      addUser(result.channels.channel, {
        title: 'title',
        parentId: 'video',
        contexts: 'video',
      })
      addUser(result.channels.channel, {
        title: 'title',
        parentId: 'audio',
        contexts: 'audio',
      })
    }
  })
}

function addUser(data, info) {
  data.forEach((item) => {
    var tempContextMenuItem = {
      'id': `${info.parentId}/${item.id}/${item.AccessHash}`,
      'title': item[info.title],
      'parentId': info.parentId,
      'contexts': [info.contexts],
    }
    chrome.contextMenus.create(tempContextMenuItem)
  })
}

chrome.contextMenus.onClicked.addListener((data) => {
  if (
    data.menuItemId.includes('img') ||
    data.menuItemId.includes('video') ||
    data.menuItemId.includes('audio')
  ) {
    send(data)
  }
  if (data.menuItemId.includes('select')) {
    console.log(data)
  }
})

async function send(data) {
  connectAndGetMsg()
  let req = 'tgsender:/'
  if (
    data.mediaType == 'image' ||
    data.mediaType == 'video' ||
    data.mediaType == 'audio'
  ) {
    req += `/downloadImg=${toAscii(data.srcUrl)},${toAscii(data.pageUrl)}`
  }
  if (!data.menuItemId.includes('download')) {
    req += `/auth=${toAscii(await getDataFromStorage('phoneNumber'))}`
    req += `/sendFile=${toAscii(data.menuItemId.split('/')[1])},${toAscii(
      data.menuItemId.split('/')[2]
    )}`
  }
  //console.log(req)
  window.open(req, '_blank')
}
function getDataFromStorage(key) {
  return new Promise((resolve, reject) => {
    try {
      chrome.storage.local.get(key, function (value) {
        resolve(value[key])
      })
    } catch (ex) {
      reject(ex)
    }
  })
}
function toAscii(s) {
  let text = ''
  s.split('').forEach((item) => {
    text += '$' + item.charCodeAt(0)
  })
  return text
}
