import React from 'react';
import PropTypes from 'prop-types';
import shortid from 'shortid';

export default class ListView extends React.Component {
    render() {
        return (
            <div>
                {this.props.facts.map(fact => <div key={shortid.generate()}> {fact} </div>)}
            </div>
        );
    }
}

ListView.propTypes = {
    facts: PropTypes.array.isRequired
}