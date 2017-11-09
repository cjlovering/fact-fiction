import React from 'react';
import PropTypes from 'prop-types';
import _ from '../../stylesheets/components/_FactCard.scss'

export default class FactCard extends React.Component {
    render() {
        return (
            <div className="fact-card">
                <div>
                    {this.props.fact}
                </div>
                <div className="vote-buttons">
                </div>
            </div>
        );
    }
}

FactCard.propTypes = {
    fact: PropTypes.string.isRequired
}