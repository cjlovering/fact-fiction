import React from 'react';
import PropTypes from 'prop-types';
import _ from '../../stylesheets/components/_FactCard.scss'

export default class FactCard extends React.Component {
    render() {
        const { sentence } = this.props;
        return (
            <div className="fact-card">
                <div>
                    {sentence}
                </div>
                <div className="vote-buttons" />
            </div>
        );
    }
}

FactCard.propTypes = {
    sentence: PropTypes.string.isRequired
}