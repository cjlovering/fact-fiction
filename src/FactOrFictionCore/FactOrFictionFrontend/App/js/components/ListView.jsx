import React from 'react';
import PropTypes from 'prop-types';
import shortid from 'shortid';
import FactCard from './FactCard';

export default class ListView extends React.Component {
    render() {
        return (
            <div>
                {this.props.facts.map(fact => <FactCard fact={fact} key={shortid.generate()} />)}
            </div>
        );
    }
}

ListView.propTypes = {
    facts: PropTypes.array.isRequired
}