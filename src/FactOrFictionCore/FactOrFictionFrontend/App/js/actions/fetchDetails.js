import 'whatwg-fetch';

const fetchDetails = tokenId => {
    return (dispatch) => {
        return fetch(`/Sentences/Details/${tokenId}`, {
            method: "GET",
            credentials: 'same-origin'
        })
        .then(
            response => response.json(),
            error => console.log('An error occured when fetching text entries.', error)
        )
        .then(json => {
            console.log(json);
        })
    }
}

export { fetchDetails };