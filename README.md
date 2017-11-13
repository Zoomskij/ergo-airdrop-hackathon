# ergo-airdrop-hackathon
http://ergo-airdrop-hackaton.azurewebsites.net/api/blocks/count/100/blockno/300/start/1/end/1000

count - count of winners
blockno - get hash
start - min random
end - max random

====== Post method for get winners with priority =======

http://ergo-airdrop-hackaton.azurewebsites.net/api/blocks/winners/3/coins/5/blockno/1234

Body JSON
{
     "Wallet1": 1,
     "Wallet2": 2,
     "wallet3": 3
 }
