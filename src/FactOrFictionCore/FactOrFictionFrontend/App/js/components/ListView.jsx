import React from 'react';
import PropTypes from 'prop-types';
import shortid from 'shortid';
import FactCard from './FactCard';
import _ from '../../stylesheets/components/_ListView.scss';

export default class ListView extends React.Component {
    render() {
        return (
            <div className='list-view'>
                {this.props.facts.map(fact => <FactCard fact={fact} key={shortid.generate()} />)}
            </div>
        );
    }
}

ListView.propTypes = {
    facts: PropTypes.array.isRequired
}